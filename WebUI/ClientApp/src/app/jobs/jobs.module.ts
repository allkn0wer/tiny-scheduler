import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JobsListComponent } from './components/jobs-list/jobs-list.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', component: JobsListComponent },
];

@NgModule({
  declarations: [
    JobsListComponent
  ],
  imports: [
    CommonModule, SharedModule, RouterModule.forChild(routes),
  ]
})
export class JobsModule { }
