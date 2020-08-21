import React, { useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import addIcon from '../../../assets/add.svg';
import closeIcon from '../../../assets/close.svg';
import removeIcon from '../../../assets/remove.svg';
import { Modal } from '../../../components';
import { BoardMember } from '../../../models';
import { apiGet, isErrorResponse } from '../../../services/kanbanApiService';
import {
    AddMemberWrapper,
    CloseButton,
    EmailInput,
    Header,
    IconButton,
    MemberCard,
    MemberItem,
    MembersList,
} from './styles';

interface IMembersModalProps {
    onClose(): void;
}

export const MembersModal: React.FC<IMembersModalProps> = ({ onClose }) => {
    const [userEmail, setUserEmail] = useState('');
    const [members, setMembers] = useState<BoardMember[]>([]);
    const [token] = useState(sessionStorage.getItem('jwtToken') ?? '');
    const history = useHistory();
    const { boardId } = useParams();

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
    }, [boardId, history, token, onClose]);

    const handleAddMember = () => {};
    const handleRemoveMember = (member: BoardMember) => {};

    return (
        <Modal
            title={
                <Header>
                    <CloseButton
                        src={closeIcon}
                        alt="Close"
                        onClick={onClose}
                    />
                    <AddMemberWrapper>
                        <EmailInput
                            placeholder="User email"
                            value={userEmail}
                            onChange={(e) => setUserEmail(e.target.value)}
                        />
                        <IconButton
                            src={addIcon}
                            alt="Add"
                            onClick={handleAddMember}
                        />
                    </AddMemberWrapper>
                </Header>
            }
        >
            <MembersList>
                {members.map((member) => (
                    <MemberItem key={member.id}>
                        <MemberCard>{`${member.name} (${member.email})`}</MemberCard>
                        <IconButton
                            src={removeIcon}
                            alt="Remove"
                            onClick={(e) => handleRemoveMember(member)}
                        />
                    </MemberItem>
                ))}
            </MembersList>
        </Modal>
    );
};
