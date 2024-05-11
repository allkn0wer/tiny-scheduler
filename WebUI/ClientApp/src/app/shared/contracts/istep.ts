export interface IStep {
  id: number;
  name?: string;
  order?: number;
  type?: string;
  breakOnError?: boolean;
  retries?: number;
  script?: string;
  parentStepId?: number | null;
  jobId?: number;
  steps?: Array<IStep>;
}
