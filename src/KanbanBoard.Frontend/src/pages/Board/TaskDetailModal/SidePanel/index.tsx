import React, { useContext, useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import assignmentIcon from '../../../../assets/assignment.svg';
import deleteIcon from '../../../../assets/delete_outline.svg';
import { DeleteModal } from '../../../../components';
import { BoardContext } from '../../../../contexts';
import { Task, TaskList, User } from '../../../../models';
import {
    apiDelete,
    isErrorResponse,
} from '../../../../services/kanbanApiService';
import { EditableTagColor } from '../EditableTagColor';
import { EditAssignments } from '../EditAssignments';
import {
    AssignedMemberName,
    AssignmentSectionTitle,
    Button,
    SectionTitle,
    SidePanelWrapper,
} from './styles';

interface ISidePanelProps {
    task?: Task;
}

export const SidePanel: React.FC<ISidePanelProps> = ({ task }) => {
    const [showAssignmentsModal, setShowAssignmentsModal] = useState(false);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [taskAssignments, setTaskAssignments] = useState<User[]>([]);
    const boardContext = useContext(BoardContext);
    const history = useHistory();
    const { boardId } = useParams();

    useEffect(() => setTaskAssignments(task?.assignments ?? []), [task]);

    const handleTaskDeletion = async () => {
        setShowDeleteModal(false);
        const taskId = task?.id ?? 0;
        const token = sessionStorage.getItem('jwtToken') ?? '';

        const response = await apiDelete({
            uri: `v1/boards/${boardId}/tasks/${taskId}`,
            bearerToken: token,
        });
        if (response.data && isErrorResponse(response.data)) {
            if (response.data.status === 401 || response.data.status === 403) {
                history.push('/login');
            }
            return;
        }
        const listId = task?.list ?? 0;
        const updateList = (lists: TaskList[]) => {
            const listWithContainsTheTask = lists.find(
                (list) => list.id === listId
            );
            if (!listWithContainsTheTask) {
                return lists;
            }
            const filteredLists = lists.filter((list) => list.id !== listId);
            const updatedList = {
                ...listWithContainsTheTask,
                tasks: listWithContainsTheTask.tasks.filter(
                    (_tasks) => _tasks.id !== taskId
                ),
            };
            const finalLists = [...filteredLists, updatedList];
            return finalLists.sort((a, b) => a.id - b.id);
        };
        boardContext.setLists(updateList(boardContext.lists));

        history.push(`/board/${boardId}`);
    };

    return (
        <>
            <SidePanelWrapper>
                <SectionTitle>Tag</SectionTitle>
                <EditableTagColor tagColor={task?.tagColor ?? ''} />
                <Button onClick={() => setShowDeleteModal(true)}>
                    <img src={deleteIcon} alt="Delete" /> DELETE TASK
                </Button>
                <AssignmentSectionTitle>
                    <img src={assignmentIcon} alt="Assignments" /> Assignments
                </AssignmentSectionTitle>
                <Button onClick={() => setShowAssignmentsModal(true)}>
                    EDIT ASSIGNMENTS
                </Button>
                {taskAssignments.map((assignedMember, index) => (
                    <AssignedMemberName key={index}>
                        {assignedMember.name}
                    </AssignedMemberName>
                ))}
            </SidePanelWrapper>
            {showAssignmentsModal && task && (
                <EditAssignments
                    taskAssignments={taskAssignments}
                    onAssignmentsChange={setTaskAssignments}
                    onClose={() => setShowAssignmentsModal(false)}
                />
            )}
            {showDeleteModal && (
                <DeleteModal
                    onCancel={() => setShowDeleteModal(false)}
                    onConfirm={handleTaskDeletion}
                    headerText="Delete the task?"
                >
                    <>
                        Are you sure that you want to delete the Task?
                        <br />
                        This will also remove all the related assignments.
                    </>
                </DeleteModal>
            )}
        </>
    );
};
