import React from 'react';
import Button from 'react-bootstrap/Button';
import TaskDetails from "./TaskDetails";
import Navbar from 'react-bootstrap/Navbar';
import Container from 'react-bootstrap/Container';
import { ButtonGroup, Nav } from 'react-bootstrap';
import ColumnDetails from './ColumnDetails';
import ColumnNames from './ColumnNames';
import PropTypes from 'prop-types';

function Controls(props) {
    const [modalShowTask, setModalShowTask] = React.useState(false);
    const [modalShowColumn, setModalShowColumn] = React.useState(false);
  
    return (
      <div className="h-100">
        <div>
          <Navbar bg="dark" expand="lg" variant="dark" fixed="top">
              <Container>
                  <Navbar.Brand className="justify-content-start">
                    MyToDos
                  </Navbar.Brand>
                  <Nav className = "justify-content-end">
                    <ButtonGroup aria-label = "controlButtons">
                      <Button variant="dark" className = "simpleBtn" onClick = {() => setModalShowTask(true)}>
                        Add new task
                      </Button>
                      <Button variant="dark" className = "simpleBtn" onClick = {() => setModalShowColumn(true)}>
                        Add new column
                      </Button>
                    </ButtonGroup>
                  </Nav>
              </Container>
          </Navbar>
        </div>
        <div>
          <TaskDetails 
              mode = "add"
              show = {modalShowTask}
              onHide={() => setModalShowTask(false)}
              task = {{
                  id: "",
                  title: "",
                  description: "",
                  deadline: "",
                  columnID: ColumnNames.getAll()[0].id,
              }}
              add = {props.addNewTask}
          /> 
          <ColumnDetails
              mode = "add"
              show = {modalShowColumn}
              onHide = {() => setModalShowColumn(false)}
              column = {{
                id: "",
                name: "",
                priority: "",
              }}
              add = {props.addNewColumn}
          />
        </div>
      </div>
    );
}

Controls.propTypes = {
  addNewTask: PropTypes.func,
  addNewColumn: PropTypes.func,
}

export default Controls;