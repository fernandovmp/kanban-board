import React from 'react';
import { useHistory } from 'react-router-dom';
import { SummarizedTask } from '../../../../models';
import { Tag, TaskWrapper } from './styles';

interface ITaskCardProps {
    task: SummarizedTask;
}

export const TaskCard: React.FC<ITaskCardProps> = ({ task }) => {
    const history = useHistory();

    const handleClick = () => {
        history.push(`${history.location.pathname}/task/${task.id}`);
    };

    return (
        <TaskWrapper onClick={handleClick}>
            <Tag color={`#${task.tagColor}`} />
            {task.summary}
        </TaskWrapper>
    );
};
