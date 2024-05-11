import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IJob } from '../contracts/ijob';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiGateway {

  private readonly API_BASE_URL = '/api';

  constructor(private _http: HttpClient) { }

  public getJobs(): Observable<IJob[]> {
    return this._http.get<IJob[]>(this.API_BASE_URL + '/job');
  }
}
