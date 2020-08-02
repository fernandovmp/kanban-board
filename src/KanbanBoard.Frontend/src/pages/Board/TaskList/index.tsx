import React, { useState } from 'react';
import addIcon from '../../../assets/add.svg';
import { EditableContent } from '../../../components/EditableContent';
import { TaskList } from '../../../models';
import {
    Button,
    ButtonsWrapper,
    CancelButton,
    ListTitle,
    ListWrapper,
    NewTaskInput,
    SaveButton,
} from './styles';
import { TaskCard } from './TaskCard';
import { Tag, TaskWrapper } from './TaskCard/styles';

interface ITaskListProps {
    taskList: TaskList;
}

export const TaskListView: React.FC<ITaskListProps> = ({ taskList }) => {
    const [listTitle, setListTitle] = useState(taskList.title);
    const [tasks, setTasks] = useState(taskList.tasks);
    const [isCreatingTask, setIsCreatingTask] = useState(false);
    const [newTaskSummary, setNewTaskSummary] = useState('');

    const handleCreateTask = () => {
        setIsCreatingTask(false);
        if (newTaskSummary.trim() === '') return;
        const newTask = {
            id: 70,
            summary: newTaskSummary.trim(),
            tagColor: 'FFEA31',
        };
        setNewTaskSummary('');
        setTasks([...tasks, newTask]);
    };

    const handleEditListTitle = (value: string) => {
        if (value.trim() === '') return;
        setListTitle(value.trim());
    };

    return (
        <ListWrapper>
            <EditableContent
                onEndEdit={handleEditListTitle}
                initialInputValue={listTitle}
            >
                <ListTitle>{listTitle}</ListTitle>
            </EditableContent>
            {tasks.map((task) => (
                <TaskCard key={task.id} task={task} />
            ))}
            {isCreatingTask ? (
                <>
                    <TaskWrapper>
                        <Tag color="#FFEA31" />
                        <NewTaskInput
                            value={newTaskSummary}
                            onChange={(e) => setNewTaskSummary(e.target.value)}
                        />
                    </TaskWrapper>
                    <ButtonsWrapper>
                        <CancelButton onClick={() => setIsCreatingTask(false)}>
                            CANCEL
                        </CancelButton>
                        <SaveButton onClick={handleCreateTask}>SAVE</SaveButton>
                    </ButtonsWrapper>
                </>
            ) : (
                <Button onClick={() => setIsCreatingTask(true)}>
                    <img src={addIcon} alt="New Task" />
                    New Task
                </Button>
            )}
        </ListWrapper>
    );
};
