export abstract class JobBase {
  public name: string | null = null;
  public cron: string | null = null;
  public comment: string | null = null;
  public isActive: boolean = true;
}
