import React from 'react';
import Axios from 'axios';

const api = Axios.create({
    baseURL: 'http://localhost:5000/api'
});


class ApiManager extends React.Component {

    static async getAllColumns() {
      const data = [];
      await api.get('/column').then((response) => {
        console.log(response);
        for (let i = 0; i < response.data.length; i++) {
          data.push(response.data[i]);
        }
      })
      return data;
    }

    static async getAllTasks() {
      const data = [];
      await api.get('/todo').then((response) => {
        console.log(response);
        for (let i = 0; i < response.data.length; i++) {
          data.push(response.data[i]);
        }
      })
      return data;
    }
  
    static async addNewTask(task) {
      var taskResponse;
      await api.post('/todo', {
        title: task.title,
        description: task.description,
        deadline: task.deadline,
        priority: task.priority,
        columnID: task.columnID,
      }).then((response) => {
        console.log(response);
        taskResponse = response;
      });
      return taskResponse.data;
    }
  
    static async deleteTask(task) {
      await api.delete('/todo/' + task.id).then((response) => {
        console.log(response);
      });
    }
  
    static async updateTask(task) {
      var taskResponse;
      await api.put('/todo/' + task.id, task).then((response) => {
        console.log(response);
        taskResponse = response;
      });
      return taskResponse.data;
    }

    static async updateTaskPriority(task) {
      var taskResponse;
      await api.put('/todo/' + task.id + '/priority', task).then((response) => {
        console.log(response);
        taskResponse = response;
      })
      return taskResponse.data;
    }

    static async addNewColumn(column) {
      var columnResponse;
      await api.post('/column', {
        name: column.name,
        priority: column.priority,
      }).then((response) => {
        console.log(response);
        columnResponse = response;
      });
      return columnResponse.data;
    }

    static async deleteColumn(column) {
      await api.delete('/column/' + column.id, column).then((response) => {
        console.log(response);
      });
    }

    static async updateColumn(column) {
      var columnResponse;
      await api.put('/column/' + column.id, column).then((response) => {
        console.log(response);
        columnResponse = response;
      });
      return columnResponse.data;
    }

    static async updateColumnPriority(column) {
      var columnResponse;
      await api.put('/todo/' + column.id + '/priority', column).then((response) => {
        console.log(response);
        columnResponse = response;
      })
      return columnResponse.data;
    }

  }
  export default ApiManager;