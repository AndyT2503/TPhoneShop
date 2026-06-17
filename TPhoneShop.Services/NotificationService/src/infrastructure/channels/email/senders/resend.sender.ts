import { Injectable, Scope } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { EmailSender } from './email.sender';
import { Resend } from 'resend';

@Injectable({ scope: Scope.REQUEST })
export class ResendSender implements EmailSender {
  private readonly fromEmail: string;
  private readonly resend!: Resend;

  constructor(private readonly configService: ConfigService) {
    const apiKey = this.configService.get<string>('resend.apiKey');
    if (!apiKey) {
      throw new Error('RESEND_API_KEY is not configured');
    }

    this.resend = new Resend(apiKey);
    this.fromEmail = this.configService.get<string>('resend.fromEmail')!;
  }

  async send(to: string, subject: string, html: string): Promise<void> {
    await this.resend.emails.send({
      to,
      from: this.fromEmail,
      subject,
      html,
    });
  }
}
