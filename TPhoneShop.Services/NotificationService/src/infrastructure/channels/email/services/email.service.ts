import { Injectable, Scope } from '@nestjs/common';
import { EmailSender } from '../senders/email.sender';
import { NotificationEvent } from 'src/domain/enums';
import * as path from 'path';
import * as Handlebars from 'handlebars';
import { readFile } from 'fs/promises';

@Injectable({
  scope: Scope.REQUEST,
})
export class EmailService {
  constructor(private readonly emailSender: EmailSender) {}

  async sendEmail(
    recipientEmail: string,
    event: NotificationEvent,
    payload: Record<string, unknown>,
  ): Promise<void> {
    const template = await this.renderTemplate(event, payload);
    const subject = this.getEmailSubject(event);

    await this.emailSender.send(recipientEmail, subject, template);
  }

  private async renderTemplate(
    event: NotificationEvent,
    payload: Record<string, unknown>,
  ): Promise<string> {
    const templatePath = path.join(
      process.cwd(),
      'src/infrastructure/channels/email/templates',
    );
    const filePath = path.join(templatePath, `${event}.html`);
    const source = await readFile(filePath, 'utf-8');
    const compiled = Handlebars.compile(source);
    return compiled(payload);
  }

  private getEmailSubject(event: NotificationEvent): string {
    switch (event) {
      case NotificationEvent.FORGOT_PASSWORD:
        return '[TPhoneShop] Khôi phục mật khẩu';

      default:
        return '[TPhoneShop] Thông báo từ hệ thống';
    }
  }
}
