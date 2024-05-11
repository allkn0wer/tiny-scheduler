import { IJob } from "../../shared/contracts/ijob";
import { JobBase } from "../../shared/model/job-base.model";
import { Step } from "./step.model";

export class Job extends JobBase {

  private _currentNewStepId = 0;
  public id: number | null = null;
  public steps: Array<Step> = [];

  constructor(iJob: Partial<IJob> | null) {
    super();
    if (!iJob) {
      return;
    }
    Object.assign(this, iJob);

    this.id = iJob.id ?? 0;
    this.isActive = iJob.isActive ?? true;
    this.steps = (iJob.steps ?? []).map(s => new Step(s));
  }

  public toServerObject(): IJob {
    const serverJob = Object.assign({}, this) as IJob;
    serverJob.steps = this.steps.map(s => s.toServerObject());
    return serverJob;
  }
}
