import { Component, Inject } from '@angular/core';
import {task} from './taskInterface'
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent {

  // Needed to create the three coloumns
  tiles = [
    {text: 'Todo', cols: 4, rows: 5, color: 'lightblue'},
    {text: 'In Progress', cols: 4, rows: 5, color: 'lightpink'},
    {text: 'Done', cols: 4, rows: 5, color: 'lightgreen'},
  ];

  // Prepopulated array of tasks to get rendered in the Todo coloumn
  tasks: task[] = [
    {id: 0, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 1", assignee:""},
    {id: 1, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 2", assignee:""},
    {id: 2, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 3", assignee:""},
    {id: 3, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 4", assignee:""},
    {id: 4, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 5", assignee:""},
    {id: 5, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 6", assignee:""},
  ]

  constructor(public dialog: MatDialog) {}

  // Updates the task's status that is passed in based on the action that is specified
  updateTask(task, action): void{
    var index = this.tasks.indexOf(task);
    if(action == 'assign')
      this.tasks[index].status = "In Progress";
    else
      this.tasks[index].status = "Done";
  }

  // Opens the modal for assigning tasks
  openDialog(task): void {
    let dialogRef = this.dialog.open(TaskDialogComponent, {
      width: '250px',
      data: { assignee: ""}
    });

    // Subscribes to afterClose async function to get the result of the dialog
    dialogRef.afterClosed().subscribe(result => {
      this.tasks[this.tasks.indexOf(task)].assignee = result;
      if(result && result.replace(/\s/g, '').length)
        this.updateTask(task,'assign');
    });
  }

  

}

// The component used for creating the dialog for assigning tasks
@Component({
  selector: 'task-dialog',
  templateUrl: './task-dialog.component.html',
})
export class TaskDialogComponent {

  // The @inject is used to access data in te dialog 
  constructor(
    public dialogRef: MatDialogRef<TaskDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  //Closes the dialog
  onNoClick(): void {
    this.dialogRef.close();
  }

}
