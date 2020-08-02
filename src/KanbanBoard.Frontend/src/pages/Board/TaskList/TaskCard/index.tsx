import React from 'react';
import { SummarizedTask } from '../../../../models';
import { Tag, TaskWrapper } from './styles';

interface ITaskCardProps {
    task: SummarizedTask;
}

export const TaskCard: React.FC<ITaskCardProps> = ({ task }) => {
    return (
        <TaskWrapper>
            <Tag color={`#${task.tagColor}`} />
            {task.summary}
        </TaskWrapper>
    );
};
