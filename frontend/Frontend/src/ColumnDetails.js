import React from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import PropTypes from 'prop-types';

class ColumnDetails extends React.Component {
    constructor(props) {
      super(props);
      this.state = {
        mode: this.props.mode,
        id: this.props.column.id,
        columnName: this.props.column.name,
        priority: this.props.column.priority,
      }
      this.handleInputChange = this.handleInputChange.bind(this);
      this.handleOpen = this.handleOpen.bind(this);
      this.handleCloseClick = this.handleCloseClick.bind(this);
    }
  
    handleOpen() {
      this.setState({
        columnName: this.props.column.name,
        priority: this.props.column.priority,
      });
    }

    handleCloseClick (skip = false) {
      if (!skip) {
        this.setState({
          columnName: this.props.column.name,
          priority: this.props.column.priority,
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
          name: this.state.columnName,
          priority: this.state.priority,
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
            name: this.state.columnName,
            priority: this.state.priority,
        });
      }}
      >
        Delete column
      </Button>
  
      let addButton = 
      <Button
      key = "add"
      variant = "outline-success" 
      onClick = {() => {
        this.handleCloseClick(true);
        this.props.add({
            id: this.state.id,
            name: this.state.columnName,
            priority: this.state.priority,
        });
      }}
      >
        Add column
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
            {this.state.mode === "edit" ? "Edit column" : "Add new column"}
          </Modal.Header>
          <Modal.Body>
          <InputGroup className="mb-1">
          <InputGroup.Text id="basic-addon1">Name:</InputGroup.Text>
          <Form.Control
            name = "columnName"
            placeholder="Name"
            aria-describedby="basic-addon1"
            type = "text"
            value = {this.state.columnName}
            onChange = {this.handleInputChange}
            autoComplete = "off"
          />
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

ColumnDetails.propTypes = {
  add: PropTypes.func,
  change: PropTypes.func,
  column: PropTypes.object,
  delete: PropTypes.func,
  mode: PropTypes.string,
  onHide: PropTypes.func,
  show: PropTypes.bool,
}
export default ColumnDetails;