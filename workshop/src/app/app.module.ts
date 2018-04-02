import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatGridListModule, MatCardModule, MatDialogModule, MatFormFieldModule, MatButtonModule, MatInputModule} from '@angular/material'
import { AppComponent } from './app.component';
import { TaskListComponent, TaskDialogComponent} from './task-list/task-list.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

const appRoutes: Routes = [
  { path: '', component: TaskListComponent },
  { path: '**',  redirectTo: ''},
];

@NgModule({
  declarations: [
    AppComponent,
    TaskListComponent,
    TaskDialogComponent
  ],
  imports: [
    BrowserModule,
    MatCardModule,
    MatGridListModule,
    MatDialogModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    RouterModule.forRoot(
      appRoutes,
    ),
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [TaskListComponent, TaskDialogComponent]
  
})
export class AppModule { }
``