import React from 'react';
import { useHistory } from 'react-router-dom';
import { SummarizedTask } from '../../../../models';
import { Tag, TaskWrapper } from './styles';

interface ITaskCardProps {
    task: SummarizedTask;
    listId: number;
}

export const TaskCard: React.FC<ITaskCardProps> = ({ task, listId }) => {
    const history = useHistory();

    const handleClick = () => {
        history.push(`${history.location.pathname}/task/${task.id}`);
    };

    const handleDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData('task', JSON.stringify(task));
        event.dataTransfer.setData('listId', listId.toString());
        event.dataTransfer.dropEffect = 'move';
    };

    return (
        <TaskWrapper
            onClick={handleClick}
            draggable
            onDragStart={handleDragStart}
        >
            <Tag color={`#${task.tagColor}`} />
            {task.summary}
        </TaskWrapper>
    );
};
