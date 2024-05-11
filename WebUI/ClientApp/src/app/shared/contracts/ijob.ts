import { IStep } from "./istep";

export interface IJob {
  id?: number;
  name?: string;
  cron?: string;
  comment?: string;
  isActive?: boolean;
  steps?: Array<IStep>;
}
