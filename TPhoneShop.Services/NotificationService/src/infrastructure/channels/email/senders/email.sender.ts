export abstract class EmailSender {
  abstract send(to: string, subject: string, html: string): Promise<void>;
}
