import React from 'react';
import Card from 'react-bootstrap/Card';
import TaskDetails from "./TaskDetails";
import Button from 'react-bootstrap/Button';
import PropTypes from 'prop-types';

function Task(props) {
    const [modalShow, setModalShow] = React.useState(false);
  
    return (
      <div>
        <div className = "taskField">
          <div className="d-grid gap-2">
            <Card className="taskCard">
              <Card.Title>{props.task.title}</Card.Title>
              <Card.Text>{props.task.description}</Card.Text>
              <Card.Subtitle  className="d-flex justify-content-between">
                {props.task.deadline}
                <TaskDetails 
                  mode = "edit"
                  show = {modalShow}
                  onHide = {() => setModalShow(false)}
                  task = {props.task}
                  change = {props.updateTask}
                  delete = {props.deleteTask}
                />
                <Button variant="outline-primary" size="sm" className = "simpleBtn" onClick = {() => setModalShow(true)}>
                    Edit
                </Button> 
              </Card.Subtitle>
            </Card>
          </div>
        </div>
      </div>
    );
}

Task.propTypes = {
  deleteTask: PropTypes.func,
  task: PropTypes.object,
  updateTask: PropTypes.func,
}

export default Task;