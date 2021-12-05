import React from 'react';
import Task from './Task';
import Col from 'react-bootstrap/Col'
import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import { DragDropContext, Draggable, Droppable } from 'react-beautiful-dnd';
import TaskDetails from './TaskDetails';
import './index.css'
import ColumnNames from './ColumnNames';
import ColumnDetails from './ColumnDetails';
import PropTypes from 'prop-types';

function Column(props) {

    const [modalShowTask, setModalShowTask] = React.useState(false);
    const [modalShowColumn, setModalShowColumn] = React.useState(false);
    const [tasks, updateTasksView] = React.useState(props.column.tasks);
    React.useEffect(() => {
        updateTasksView(props.column.tasks);
    }, [props.column.tasks]);
    
    function handleOnDragEnd(result) {
        if ((result.destination === null) || (result.source.index === result.destination.index)) 
            return;
        const items = Array.from(tasks);
        const[reorderedItem] = items.splice(result.source.index, 1);
        items.splice(result.destination.index, 0, reorderedItem);
        updateTasksView(items);
        let updatedTask = {
            id: reorderedItem.id,
            title: reorderedItem.title,
            description: reorderedItem.description,
            deadline: reorderedItem.deadline,
            priority: result.destination.index,
            columnID: reorderedItem.columnID,
        };
        props.updateTaskPriority(updatedTask);
    }

    return(
        <Col>
            <Card className="taskList" bg="light">
                <Card.Header className="d-flex justify-content-between">
                    <b>{props.column.name}</b>
                    <TaskDetails 
                        mode = "add"
                        show = {modalShowTask}
                        onHide={() => setModalShowTask(false)}
                        task = {{
                            id: "",
                            title: "",
                            description: "",
                            deadline: "",
                            columnID: ColumnNames.getAll().filter(c => c.name === props.column.name)[0].id,
                        }}
                        add = {props.addNewTask}
                    /> 
                    <ColumnDetails 
                        mode = "edit"
                        show = {modalShowColumn}
                        onHide = {() => setModalShowColumn(false)}
                        column = {props.column}
                        change = {props.updateColumn}
                        delete = {props.deleteColumn}
                    />
                    <ButtonGroup aria-label="Column functions">
                        <Button variant="outline-dark" size="sm" className = "simpleBtn" onClick = {() => setModalShowColumn(true)}>
                            Edit
                        </Button>
                        <Button variant="secondary" size="sm" className = "simpleBtn" onClick = {() => setModalShowTask(true)}>
                            New task
                        </Button>
                    </ButtonGroup>
                </Card.Header>
                <Card.Body>  
                    <DragDropContext onDragEnd = {handleOnDragEnd}>
                        <Droppable droppableId = "tasks">
                            {(provided) => (
                                <ul className = "tasks no-bullets" {...provided.droppableProps} ref = {provided.innerRef}>
                                    {tasks.map((task, index) => {
                                        return (
                                            <Draggable key = {task.id} draggableId = {task.id.toString()} index = {index}>
                                                {(provided) => (
                                                    <li {...provided.draggableProps} {...provided.dragHandleProps} ref ={provided.innerRef}>
                                                    <Task 
                                                        task = {task}
                                                        key = {task.id}
                                                        deleteTask = {props.deleteTask}
                                                        updateTask = {props.updateTask}
                                                    />
                                                    </li>
                                                )}
                                            </Draggable>
                                        );
                                    })}
                                    {provided.placeholder}
                                </ul>
                            )}
                        </Droppable>
                    </DragDropContext>
                </Card.Body>
            </Card>
        </Col>
    );     
}

Column.propTypes = {
    addNewTask: PropTypes.func,
    column: PropTypes.object,
    deleteColumn: PropTypes.func,
    deleteTask: PropTypes.func,
    updateColumn: PropTypes.func,
    updateTask: PropTypes.func,
    updateTaskPriority: PropTypes.func,
};

export default Column;