import React, { useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import closeIcon from '../../../assets/close.svg';
import { Task } from '../../../models';
import {
    apiGet,
    apiPatch,
    isErrorResponse,
} from '../../../services/kanbanApiService';
import { getJwtToken } from '../../../services/tokenService';
import { EditableDescription } from './EditableDescription';
import { SidePanel } from './SidePanel';
import {
    CloseButton,
    DescriptionWrapper,
    EditableSummary,
    ModalCard,
    SectionTitle,
    SummaryAndDescriptionSection,
    TaskModalPanel,
    TaskSummary,
    TaskTag,
} from './styles';

export const TaskDetailModal: React.FC = () => {
    const [task, setTask] = useState<Task>();
    const [summary, setSummary] = useState('');
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
            setSummary(response.data!.summary);
        };
        fetchTask();
    }, [boardId, taskId, history]);

    const handleClose = () => {
        history.push(`/board/${boardId}`);
    };

    const preventPropagation = (
        event: React.MouseEvent<HTMLDivElement, MouseEvent>
    ) => event.stopPropagation();

    const handleEditSummary = async (value: string) => {
        const token = getJwtToken();
        const newSummary = value.trim();
        if (newSummary === '') return;
        const response = await apiPatch({
            uri: `v1/boards/${boardId}/tasks/${taskId}`,
            body: {
                summary: newSummary,
            },
            bearerToken: token,
        });
        if (response.data && isErrorResponse(response.data)) {
            if (response.data.status === 401 || response.data.status === 403) {
                history.push('/login');
            }
            return;
        }
        setSummary(value);
    };

    return (
        <TaskModalPanel onClick={handleClose}>
            <ModalCard onClick={preventPropagation}>
                <CloseButton
                    src={closeIcon}
                    alt="Close"
                    onClick={handleClose}
                />
                <SummaryAndDescriptionSection>
                    <EditableSummary
                        initialInputValue={summary}
                        onEndEdit={handleEditSummary}
                    >
                        <TaskSummary>{summary}</TaskSummary>
                    </EditableSummary>
                    <TaskTag
                        color={
                            task !== undefined
                                ? `#${task.tagColor}`
                                : 'transparent'
                        }
                    />
                    <DescriptionWrapper>
                        <SectionTitle>Description</SectionTitle>
                        <EditableDescription
                            taskDescription={task?.description ?? ''}
                        />
                    </DescriptionWrapper>
                </SummaryAndDescriptionSection>
                <SidePanel task={task} />
            </ModalCard>
        </TaskModalPanel>
    );
};
