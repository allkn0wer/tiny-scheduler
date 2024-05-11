import { IStep } from "../../shared/contracts/istep";
import { StepBase } from "../../shared/model/step-base.model";

export class Step extends StepBase {
  public jobId: number = 0;
  public steps: Step[] = [];

  constructor(iStep: IStep) {
    super();
    if (!iStep)
      return;

    Object.assign(this, iStep);

    this.id = iStep.id;
    this.jobId = iStep.jobId ?? 0;
    this.retries = iStep.retries ?? 0;
    this.breakOnError = iStep.breakOnError ?? false;
    this.steps = iStep.steps?.map(s => new Step(s)) ?? [];
  }

  public toServerObject(): IStep {
    const step = Object.assign({}, this) as IStep;
    step.id = this.id < 0 ? 0 : this.id;
    step.steps = this.steps?.map(s => s.toServerObject());
    return step;
  }
}
