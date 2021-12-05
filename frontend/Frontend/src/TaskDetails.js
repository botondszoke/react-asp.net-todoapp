import React from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import ColumnNames from './ColumnNames';
import PropTypes from 'prop-types';

class TaskDetails extends React.Component {
    constructor(props) {
      super(props);
      this.state = {
        mode: this.props.mode,
        id: this.props.task.id,
        title: this.props.task.title,
        description: this.props.task.description,
        deadline: this.props.task.deadline,
        priority: this.props.task.priority,
        columnNames: ColumnNames.getAll(),
        column: ColumnNames.getAll().filter(c => c.id === this.props.task.columnID)[0].name,
      }
      this.handleInputChange = this.handleInputChange.bind(this);
      this.handleOpen = this.handleOpen.bind(this);
      this.handleCloseClick = this.handleCloseClick.bind(this);
    }
  
    handleOpen() {
      this.setState({
        title: this.props.task.title,
        description: this.props.task.description,
        deadline: this.props.task.deadline,
        priority: this.props.task.priority,
        columnNames: ColumnNames.getAll(),
        column: ColumnNames.getAll().filter(c => c.id === this.props.task.columnID)[0].name,
      });
    }

    handleCloseClick(skip = false) {
      if (!skip) {
        this.setState({
          title: this.props.task.title,
          description: this.props.task.description,
          deadline: this.props.task.deadline,
          priority: this.props.task.priority,
          column: this.state.columnNames.filter(c => c.id === this.props.task.columnID)[0].name,
        });
      }
      this.props.onHide();
    }
  
    handleInputChange(event) {
      const name = event.target.name;
      this.setState({
        [name]: event.target.value,
      });
    }
  
    render() {
      let saveButton = 
      <Button
      key = "save"
      variant = "outline-success" 
      onClick = {() => {
        this.handleCloseClick(true);
        this.props.change({
          id: this.state.id,
          title: this.state.title,
          description: this.state.description,
          deadline: this.state.deadline,
          priority: this.state.priority,
          columnID: this.state.columnNames.filter(c => c.name === this.state.column)[0].id,
        });
      }}
      >
        Save changes
      </Button>;
  
      let deleteButton = 
      <Button
      key = "delete"
      variant = "outline-danger" 
      onClick = {() => {
        this.handleCloseClick();
        this.props.delete({
          id: this.state.id,
          title: this.state.title,
          description: this.state.description,
          deadline: this.state.deadline,
          priority: this.state.priority,
          columnID: this.state.columnNames.filter(c => c.name === this.state.column)[0].id,
        });
      }}
      >
        Delete task
      </Button>
  
      let addButton = 
      <Button
      key = "add"
      variant = "outline-success" 
      onClick = {() => {
        this.handleCloseClick(true);
        this.props.add({
          id: this.state.id,
          title: this.state.title,
          description: this.state.description,
          deadline: this.state.deadline,
          priority: this.state.priority,
          columnID: this.state.columnNames.filter(c => c.name === this.state.column)[0].id,
        });
      }}
      >
        Add task
      </Button>;

      let buttons = [];
      if (this.state.mode === "edit") {
        buttons.push(saveButton);
        buttons.push(deleteButton);
      }
      if (this.state.mode === "add") {
        buttons.push(addButton);
      }
      return (
        <div>
        <Modal
        show = {this.props.show}
        onHide = {this.handleCloseClick}
        onShow = {this.handleOpen}
        size="lg"
        aria-labelledby="contained-modal-title-vcenter"
        centered
        >
          <Modal.Header className="modalHeader" closeButton onClick={() => this.handleCloseClick()}>
            {this.state.mode === "edit" ? "Edit task" : "Add new task"}
          </Modal.Header>
          <Modal.Body>
          <InputGroup className="mb-1">
          <InputGroup.Text id="basic-addon1">Title:</InputGroup.Text>
          <Form.Control
            name = "title"
            placeholder="Title"
            aria-describedby="basic-addon1"
            type = "text"
            value = {this.state.title}
            onChange = {this.handleInputChange}
            autoComplete = "off"
          />
          </InputGroup>
          <InputGroup className="mb-2">
          <InputGroup.Text id="basic-addon1">Description:</InputGroup.Text>
          <Form.Control
            as = "textarea"
            name = "description"
            placeholder="Description"
            aria-describedby="basic-addon1"
            type = "text"
            value = {this.state.description}
            onChange = {this.handleInputChange}
            autoComplete = "off"
          />
          </InputGroup>
          <InputGroup className="mb-3">
          <InputGroup.Text id="basic-addon1">Deadline:</InputGroup.Text>
          <Form.Control
            name = "deadline"
            placeholder="Deadline"
            aria-describedby="basic-addon1"
            type = "date"
            value = {this.state.deadline}
            onChange = {this.handleInputChange}
          />
          <InputGroup.Text id="basic-addon1">Status:</InputGroup.Text>
          <Form.Select 
          aria-label="Select state"
          name = "column"
          value = {this.state.column}
          onChange = {this.handleInputChange}
          >
            {this.state.columnNames.map(state => {
              return (
                <option key = {state.id} value={state.name}>{state.name}</option>
              );})
            }
          </Form.Select>
          </InputGroup>
          </Modal.Body>
          <Modal.Footer>
          {buttons}
          </Modal.Footer>
        </Modal>
        </div>
      );
    }
}

TaskDetails.propTypes = {
  add: PropTypes.func,
  change: PropTypes.func,
  delete: PropTypes.func,
  mode: PropTypes.string,
  onHide: PropTypes.func,
  show: PropTypes.bool,
  task: PropTypes.object,
}

export default TaskDetails;