import React, { useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import deleteIcon from '../../../assets/delete_outline.svg';
import groupIcon from '../../../assets/group.svg';
import { DefaultButton, EditableContent } from '../../../components';
import { apiPut } from '../../../services/kanbanApiService';
import { DeleteModal } from '../DeleteModal';
import { MembersModal } from '../MembersModal';
import { BoardTitle, Header } from './styles';

interface IBoardHeaderProps {
    boardTitle: string;
}

export const BoardHeader: React.FC<IBoardHeaderProps> = ({ boardTitle }) => {
    const [title, setTitle] = useState(boardTitle);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [showMembersModal, setShowMembersModal] = useState(false);
    const { boardId } = useParams();
    const history = useHistory();

    const handleMembersClick = () => {
        setShowMembersModal(true);
    };
    const handleDeleteBoard = () => {
        setShowDeleteModal(true);
    };
    const handleConfirmDeletion = () => {};

    const handleEditBoardTitle = async (value: string) => {
        const newTitle = value.trim();
        if (newTitle === '' || newTitle === title) return;
        setTitle(newTitle);
        const token = sessionStorage.getItem('jwtToken');
        if (!token) {
            history.push('/login');
            return;
        }
        await apiPut({
            uri: `v1/boards/${boardId}`,
            bearerToken: token,
            body: {
                title: newTitle,
            },
        });
    };

    return (
        <>
            <Header>
                <EditableContent
                    initialInputValue={title}
                    onEndEdit={handleEditBoardTitle}
                >
                    <BoardTitle>{title}</BoardTitle>
                </EditableContent>
                <DefaultButton onClick={handleMembersClick}>
                    <img src={groupIcon} alt="Members" />
                    Members
                </DefaultButton>
                <DefaultButton onClick={handleDeleteBoard}>
                    <img src={deleteIcon} alt="Members" />
                    Delete board
                </DefaultButton>
            </Header>
            {showDeleteModal && (
                <DeleteModal
                    onCancel={() => setShowDeleteModal(false)}
                    onConfirm={handleConfirmDeletion}
                />
            )}
            {showMembersModal && (
                <MembersModal onClose={() => setShowMembersModal(false)} />
            )}
        </>
    );
};
