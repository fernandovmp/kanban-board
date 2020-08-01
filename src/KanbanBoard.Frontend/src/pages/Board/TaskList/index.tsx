import React, { useState } from 'react';
import addIcon from '../../../assets/add.svg';
import { EditableContent } from '../../../components/EditableContent';
import { TaskList } from '../../../models';
import { Button, ListTitle, ListWrapper } from './styles';
import { TaskCard } from './TaskCard';

interface ITaskListProps {
    taskList: TaskList;
}

export const TaskListView: React.FC<ITaskListProps> = ({ taskList }) => {
    const [listTitle, setListTitle] = useState(taskList.title);

    const handleCreateTask = () => {};

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
            {taskList.tasks.map((task) => (
                <TaskCard key={task.id} task={task} />
            ))}
            <Button onClick={handleCreateTask}>
                <img src={addIcon} alt="New Task" />
                New Task
            </Button>
        </ListWrapper>
    );
};
