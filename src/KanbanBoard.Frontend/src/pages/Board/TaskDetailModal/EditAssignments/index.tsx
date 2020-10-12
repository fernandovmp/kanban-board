import React, { useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import assignedIcon from '../../../../assets/assignment_turned_in.svg';
import { MembersList } from '../../../../components';
import { BoardMember, User } from '../../../../models';
import {
    apiDelete,
    apiGet,
    apiPut,
    isErrorResponse
} from '../../../../services/kanbanApiService';
import {
    AssignedIcon,
    AssignmentMemberCard,
    AssignmentMemberItem,
    AssignmentsOverlay,
    ButtonsWrapper,
    CancelButton,
    EmailInput,
    SaveButton
} from './styles';

interface IEditAssignmentsProps {
    taskAssignments: User[];
    onAssignmentsChange: (assignments: User[]) => void;
    onClose: () => void;
}

export const EditAssignments: React.FC<IEditAssignmentsProps> = ({
    taskAssignments,
    onAssignmentsChange,
    onClose,
}) => {
    const [userEmail, setUserEmail] = useState('');
    const [token] = useState(sessionStorage.getItem('jwtToken') ?? '');
    const [members, setMembers] = useState<BoardMember[]>([]);
    const [assignments, setAssignments] = useState<BoardMember[]>([]);
    const [unAssignments, setUnAssignments] = useState<BoardMember[]>([]);
    const history = useHistory();
    const { boardId, taskId } = useParams();

    useEffect(() => {
        const fetchMembers = async () => {
            const response = await apiGet<BoardMember[]>({
                uri: `v1/boards/${boardId}/members`,
                bearerToken: token,
            });
            if (!response.data) {
                onClose();
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
            setMembers(response.data);
        };
        fetchMembers();
    }, [boardId, token, onClose, history]);

    useEffect(() => setAssignments(taskAssignments as BoardMember[]), [
        taskAssignments,
    ]);

    const handleAssignment = (member: BoardMember) => {
        const isAssigned = assignments.some(
            (assignedUser) => assignedUser.id === member.id
        );
        if (isAssigned) {
            setUnAssignments([...unAssignments, member]);
            setAssignments(
                assignments.filter(
                    (assignedMember) => assignedMember.id !== member.id
                )
            );
            return;
        }
        setUnAssignments(
            unAssignments.filter(
                (unAssignedMember) => unAssignedMember.id !== member.id
            )
        );
        setAssignments([...assignments, member]);
    };

    const handleSave = async () => {
        const assignmentsPromises = assignments.map((member) =>
            apiPut({
                uri: `v1/boards/${boardId}/tasks/${taskId}/assignments/${member.id}`,
                bearerToken: token,
            })
        );
        const unAssignmentsPromises = unAssignments.map((member) =>
            apiDelete({
                uri: `v1/boards/${boardId}/tasks/${taskId}/assignments/${member.id}`,
                bearerToken: token,
            })
        );
        const promises = [...assignmentsPromises, ...unAssignmentsPromises];
        await Promise.all(promises);
        handleClose();
    };

    const handleClose = () => {
        onAssignmentsChange(
            assignments.map((assignment) => assignment as User)
        );
        onClose();
    };

    return (
        <AssignmentsOverlay
            showCloseButton
            onClose={onClose}
            title={
                <EmailInput
                    placeholder="User email"
                    value={userEmail}
                    onChange={(e) => setUserEmail(e.target.value)}
                />
            }
        >
            <>
                <MembersList>
                    {members.map((member) => (
                        <AssignmentMemberItem
                            key={member.id}
                            onClick={() => handleAssignment(member)}
                        >
                            <AssignmentMemberCard>{`${member.name} (${member.email})`}</AssignmentMemberCard>
                            {assignments.some(
                                (assignedUser) => assignedUser.id === member.id
                            ) && (
                                <AssignedIcon
                                    src={assignedIcon}
                                    alt="Assigned"
                                />
                            )}
                        </AssignmentMemberItem>
                    ))}
                </MembersList>
                <ButtonsWrapper>
                    <CancelButton onClick={onClose}>CANCEL</CancelButton>
                    <SaveButton onClick={handleSave}>SAVE</SaveButton>
                </ButtonsWrapper>
            </>
        </AssignmentsOverlay>
    );
};
