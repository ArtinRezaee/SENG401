export interface Task {
    id: number,
    status: string,
    cols: number,
    rows: number,
    text: string,
    assignee: string
}

// Prepopulated array of tasks to get rendered in the Todo coloumn
export var tasks: Task[] = [
    {id: 0, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 1", assignee:""},
    {id: 1, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 2", assignee:""},
    {id: 2, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 3", assignee:""},
    {id: 3, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 4", assignee:""},
    {id: 4, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 5", assignee:""},
    {id: 5, status:"Todo", cols: 4, rows: 1, text: "Finish Milestone 6", assignee:""},
]

// Needed to create the three coloumns
export var tiles = [
    {text: 'Todo', cols: 4, rows: 5, color: 'lightblue'},
    {text: 'In Progress', cols: 4, rows: 5, color: 'lightpink'},
    {text: 'Done', cols: 4, rows: 5, color: 'lightgreen'},
];