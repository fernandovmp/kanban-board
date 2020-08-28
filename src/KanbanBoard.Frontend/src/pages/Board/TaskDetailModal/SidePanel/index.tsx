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
import { EditAssignments } from '../EditAssignments';
import {
    AssignedMemberName,
    AssignmentSectionTitle,
    Button,
    Color,
    SectionTitle,
    SidePanelWrapper,
    TagColorInput,
    TagColors,
} from './styles';

const colors = [
    '#FFEA31',
    '#FF3131',
    '#0069E4',
    '#00E409',
    '#8E00E4',
    '#9F9F9F',
];

interface ISidePanelProps {
    task?: Task;
}

export const SidePanel: React.FC<ISidePanelProps> = ({ task }) => {
    const [selectedTagColor, setSelectedTagColor] = useState('');
    const [showAssignmentsModal, setShowAssignmentsModal] = useState(false);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [taskAssignments, setTaskAssignments] = useState<User[]>([]);
    const boardContext = useContext(BoardContext);
    const history = useHistory();
    const { boardId } = useParams();

    useEffect(() => setSelectedTagColor(`#${task?.tagColor}`), [task]);
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
                <TagColorInput
                    value={selectedTagColor}
                    onChange={(e) => setSelectedTagColor(e.target.value)}
                />
                <TagColors>
                    {colors.map((color, index) => (
                        <Color
                            key={index}
                            color={color}
                            onClick={() => setSelectedTagColor(color)}
                        />
                    ))}
                </TagColors>
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
