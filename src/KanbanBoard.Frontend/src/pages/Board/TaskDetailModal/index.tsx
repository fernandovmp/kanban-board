import React, { useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import closeIcon from '../../../assets/close.svg';
import { EditableContent, ModalPanel } from '../../../components';
import { Task } from '../../../models';
import { apiGet, isErrorResponse } from '../../../services/kanbanApiService';
import { SidePanel } from './SidePanel';
import {
    CloseButton,
    DescriptionCard,
    DescriptionWrapper,
    ModalCard,
    SectionTitle,
    SummaryAndDescriptionSection,
    TaskSummary,
    TaskTag,
} from './styles';

export const TaskDetailModal: React.FC = () => {
    const [task, setTask] = useState<Task>();
    const history = useHistory();
    const { boardId, taskId } = useParams();

    useEffect(() => {
        const fetchTask = async () => {
            const token = sessionStorage.getItem('jwtToken') ?? '';
            const response = await apiGet<Task>({
                uri: `v1/boards/${boardId}/tasks/${taskId}`,
                bearerToken: token,
            });
            if (!response) {
                return;
            }
            if (isErrorResponse(response.data)) {
                if (
                    response.data.status === 401 ||
                    response.data.status === 403
                ) {
                    history.push('/login');
                }
                return;
            }
            setTask(response.data!);
        };
        fetchTask();
    }, [boardId, taskId, history]);

    const handleClose = () => {
        history.push(`/board/${boardId}`);
    };

    const preventPropagation = (
        event: React.MouseEvent<HTMLDivElement, MouseEvent>
    ) => event.stopPropagation();

    const handleEditSummary = (value: string) => {};

    return (
        <ModalPanel onClick={handleClose}>
            <ModalCard onClick={preventPropagation}>
                <CloseButton
                    src={closeIcon}
                    alt="Close"
                    onClick={handleClose}
                />
                <SummaryAndDescriptionSection>
                    <EditableContent onEndEdit={handleEditSummary}>
                        <TaskSummary>{task?.summary}</TaskSummary>
                    </EditableContent>
                    <TaskTag
                        color={
                            task !== undefined
                                ? `#${task.tagColor}`
                                : 'transparent'
                        }
                    />
                    <DescriptionWrapper>
                        <SectionTitle>Description</SectionTitle>
                        <EditableContent onEndEdit={() => {}}>
                            <DescriptionCard>
                                {(task?.description.length ?? 0) > 0
                                    ? task?.description
                                    : 'Adding a description...'}
                            </DescriptionCard>
                        </EditableContent>
                    </DescriptionWrapper>
                </SummaryAndDescriptionSection>
                <SidePanel task={task} />
            </ModalCard>
        </ModalPanel>
    );
};
