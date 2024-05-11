import { SelectionModel } from '@angular/cdk/collections';
import { Component, DestroyRef, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { JobsService } from '../../services/jobs.service';
import { Job } from '../../model/job.model';

@Component({
  selector: 'app-jobs-list',
  templateUrl: './jobs-list.component.html',
  styleUrls: ['./jobs-list.component.scss']
})
export class JobsListComponent implements OnInit {
  private _totalRows = 0;

  rows$ = new BehaviorSubject<Job[]>([]);
  displayedColumns: string[] = ['select', 'id', 'name', 'cron', 'operations'];
  selection: SelectionModel<Job>;
  public isLoadingResults = false;

  constructor(private jobService: JobsService, private destroyRef: DestroyRef) {
    const initialSelection: Job[] = [];
    const allowMultiSelect = true;
    this.selection = new SelectionModel<Job>(allowMultiSelect, initialSelection);
  }

  ngOnInit(): void {
    this.onRefresh();
  }

  public onRefresh(): void {
    console.log('onRefresh');
    this.isLoadingResults = true;
    this.jobService.search()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(x => {
        this._totalRows = x.length;
        this.rows$.next(x);
        this.isLoadingResults = false;
      });
  }

  public onAdd() { }

  public onEdit(job: Job): void { }

  public onDelete(job: Job | null = null): void { }

  isAllSelected() {
    return this._totalRows === this.selection.selected.length;
  }

  toggleAllRows() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.rows$.value.forEach(row => this.selection.select(row));
  }
}
