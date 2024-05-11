export abstract class StepBase {
  public id!: number;
  public name!: string;
  public order: number | null = null;
  public type: string | null = null;
  public breakOnError: boolean = true;
  public retries: number = 0;
  public script: string | null = null;
  public parentStepId: number | null = null;
}
