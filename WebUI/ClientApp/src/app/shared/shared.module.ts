import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  declarations: [],
  imports: [
    CommonModule, RouterModule, MatButtonModule, MatCheckboxModule, MatIconModule, MatToolbarModule, MatTableModule,
    MatProgressSpinnerModule, 
  ],
  exports: [
    CommonModule, RouterModule, MatButtonModule, MatCheckboxModule, MatIconModule, MatToolbarModule, MatTableModule,
    MatProgressSpinnerModule, 
  ]
})
export class SharedModule { }
