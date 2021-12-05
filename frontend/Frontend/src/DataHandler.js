import React from 'react';
import Row from 'react-bootstrap/Row';
import Column from './Column';
import ApiManager from './ApiManager';
import Controls from './Controls'
import ColumnNames from './ColumnNames';

class DataHandler extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
          columns: [],
          tasks: [],
          loaded: false
        }
        this.initData();
    }
      
    async initData() {
        const columnData = await ApiManager.getAllColumns();
        for (let i = 0; i < columnData.length; i++) {
            let column = columnData.at(i);
            this.initColumnsAddNewColumn(column);
        }

        const taskData = await ApiManager.getAllTasks();
        for (let i = 0; i < taskData.length; i++) {
            let task = taskData.at(i);
            this.initTasksAddNewTask(task);
        }

        this.setState({
          loaded: true,
        })
    }

    updateColumnTasks() {
        let columnList = this.state.columns.slice();
        for (let i = 0; i < columnList.length; i++) {
            const column = [{
              id: columnList[i].id,
              name: columnList[i].name,
              priority: columnList[i].priority,
              tasks: [],
            }];
            let columnTasks = column[0].tasks.splice();
            for (let j = 0; j < this.state.tasks.length; j++) {
                if (column[0].id === this.state.tasks[j].columnID) {
                    columnTasks.push(this.state.tasks[j]);
                }
            }
            columnTasks.sort((a, b) => (a.priority > b.priority) ? 1 : -1)
            column[0].tasks = columnTasks;
            columnList.splice(i, 1, column[0]);
        }
        this.setState({
          columns: columnList,
        });
    }

    initColumnsAddNewColumn(column) {
        const columnList = this.state.columns.slice();
        ColumnNames.add([{id: column.id, name: column.name}])
        this.setState({
          columns: columnList.concat([{
            id: column.id,
            name: column.name,
            priority: column.priority,
            tasks: [],
          }]),
        });
    }
  
    initTasksAddNewTask(task) {
      const taskList = this.state.tasks.slice();
      let newTask = {
        id: task.id,
        title: task.title,
        description: task.description,
        deadline: task.deadline,
        priority: task.priority,
        columnID: task.columnID,
      }

      const columnList = this.state.columns.slice();
      columnList.filter(c => c.id === newTask.columnID)[0].tasks = columnList.filter(c => c.id === newTask.columnID)[0].tasks.concat([newTask]);

      this.setState({
        tasks: taskList.concat([newTask]),
        columns: columnList,
      });
    }

    async addNewTask(task) {
        let newColumnTasks = this.state.tasks.filter(t => t.columnID === task.columnID);
        let maxPriority = -1;
        newColumnTasks.forEach(t => {
          if (t.priority > maxPriority)
              maxPriority = t.priority;
        })
        task.priority = maxPriority + 1;  
      
        task = await ApiManager.addNewTask(task);
        const taskList = this.state.tasks.slice();
        let newTask = {
          id: task.id,
          title: task.title,
          description: task.description,
          deadline: task.deadline,
          priority: task.priority,
          columnID: task.columnID,
        }

        const columnList = this.state.columns.slice();
        columnList.filter(c => c.id === newTask.columnID)[0].tasks = columnList.filter(c => c.id === newTask.columnID)[0].tasks.concat([newTask]);

        this.setState({
          tasks: taskList.concat([newTask]),
          columns: columnList,
        });
    }
    
    async deleteTask(task) {
        await ApiManager.deleteTask(task);
        let taskList = this.state.tasks.slice();
        for (let i = 0; i < taskList.length; i++) {
            if (taskList[i].columnID === task.columnID && taskList[i].priority > task.priority) {
                taskList[i].priority-=1;
            }
        }

        this.setState({
          tasks: taskList.filter(t => t.id !== task.id),
        });

        this.updateColumnTasks();
    }

    async updateTask(task) {
        let oldColumnID =  this.state.tasks.filter(t => t.id === task.id)[0].columnID;
        let oldPriority = task.priority;
        let newColumnTasks = this.state.tasks.filter(t => t.columnID === task.columnID);
        if (task.columnID !== oldColumnID) {
            let maxPriority = -1;
            newColumnTasks.forEach(t => {
              if (t.priority > maxPriority)
                  maxPriority = t.priority;
            })
            task.priority = maxPriority + 1;
        }

        task = await ApiManager.updateTask(task);
        let taskList = this.state.tasks.slice();

        taskList.forEach((t, idx) => {
          if (t.id === task.id)
              taskList.splice(idx, 1, task);
        })

        if (task.columnID !== oldColumnID) {
            for (let i = 0; i < taskList.length; i++) {
                if (taskList[i].columnID === oldColumnID && taskList[i].priority > oldPriority) {
                    taskList[i].priority-=1;
                }
            }
        }

        this.setState({
          tasks: taskList,
        })

        this.updateColumnTasks();
    }
    
    async updateTaskPriority(task) {
        task = await ApiManager.updateTaskPriority(task);
        let taskList = this.state.tasks.slice();

        let oldPriority;
        for (let i = 0; i < taskList.length; i++) {
            if (taskList[i].id === task.id)
                oldPriority = taskList[i].priority;
        }
        if (oldPriority < task.priority) {
            for (let i = 0; i < taskList.length; i++) {
                if (taskList[i].priority > oldPriority && taskList[i].priority <= task.priority && taskList[i].columnID === task.columnID)
                    taskList[i].priority-=1;
            }
        }
        if (oldPriority > task.priority) {
            for (let i = 0; i < taskList.length; i++) {
                if (taskList[i].priority < oldPriority && taskList[i].priority >= task.priority && taskList[i].columnID === task.columnID)
                    taskList[i].priority+=1;
            }
        }
        taskList.forEach((t, idx) => {
          if (t.id === task.id)
              taskList.splice(idx, 1, task);
        })
        this.setState({
          tasks: taskList,
        })

        this.updateColumnTasks();
    }

    async addNewColumn(column) {
        let columnList = this.state.columns.slice();
        let maxPriority = -1;
        columnList.forEach(c => {
          if (c.priority > maxPriority)
              maxPriority = c.priority;
        })
        column.priority = maxPriority + 1;  
      
        column = await ApiManager.addNewColumn(column);
        let newColumn = {
          id: column.id,
          name: column.name,
          priority: column.priority,
          tasks: [],
        }
        ColumnNames.add([{id: column.id, name: column.name}])
        this.setState({
          columns: columnList.concat([newColumn]),
        });
    }

    async deleteColumn(column) {
        await ApiManager.deleteColumn(column);
        let taskList = this.state.tasks.slice();
        let columnList = this.state.columns.slice();
        for (let i = 0; i < columnList.length; i++) {
            if (columnList[i].priority > column.priority) {
                columnList[i].priority-=1;
            }
        }

        ColumnNames.remove({id: column.id, name: column.name});
        
        this.setState({
          tasks: taskList.filter(t => t.columnID !== column.id),
          columns: columnList.filter(c => c.id !== column.id),
        });
      
    }

    async updateColumn(column) {
        const oldTasks = this.state.columns.filter(c => c.id === column.id)[0].tasks;
        column = await ApiManager.updateColumn(column);
        let columnList = this.state.columns.slice();

        let newColumn = {
          id: column.id,
          name: column.name,
          priority: column.priority,
          tasks: oldTasks,
        }

        ColumnNames.update({id: column.id, name: column.name});

        columnList.forEach((c, idx) => {
          if (c.id === column.id)
              columnList.splice(idx, 1, newColumn);
        })

        this.setState({
          columns: columnList,
        })
    }

    render() {
        const columns = [];
        if (this.state.loaded) {
            for (let i = 0; i < this.state.columns.length; i++) {
                columns.push(
                  <Column 
                    column = {this.state.columns[i]}
                    key = {this.state.columns[i].id}
                    deleteTask = {this.deleteTask.bind(this)}
                    updateTask = {this.updateTask.bind(this)}
                    addNewTask = {this.addNewTask.bind(this)}
                    updateTaskPriority = {this.updateTaskPriority.bind(this)}
                    deleteColumn = {this.deleteColumn.bind(this)}
                    updateColumn = {this.updateColumn.bind(this)}
                  />
                );
            }
            const columnList = columns.length === 0 ? null : <Row>{columns}</Row>;
            return(
              <div>
                <div className="navigaton">
                  {
                    <Controls
                      addNewTask = {this.addNewTask.bind(this)}
                      addNewColumn = {this.addNewColumn.bind(this)}
                    />
                  }
                </div>
                <div className="columnList">
                    {columnList}
                </div>
              </div>
            );  
        }
        return(null);
    }
}
export default DataHandler;