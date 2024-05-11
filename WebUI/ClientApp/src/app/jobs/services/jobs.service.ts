import { Injectable } from '@angular/core';
import { ApiGateway } from '../../shared/services/api.gateway';
import { Job } from '../model/job.model';
import { Observable, catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JobsService {

  constructor(private _api: ApiGateway) { }

  search(): Observable<Job[]> {
    return this._api.getJobs()
      .pipe(
        map(result => result.map(j => new Job(j))),
        catchError(err => {
          console.error(err);
          return of([]);
        }),
      );
  }
}
