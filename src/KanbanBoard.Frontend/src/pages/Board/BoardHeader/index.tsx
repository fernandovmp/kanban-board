import React, { useState } from 'react';
import deleteIcon from '../../../assets/delete_outline.svg';
import groupIcon from '../../../assets/group.svg';
import { DefaultButton, EditableContent } from '../../../components';
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

    const handleMembersClick = () => {
        setShowMembersModal(true);
    };
    const handleDeleteBoard = () => {
        setShowDeleteModal(true);
    };
    const handleConfirmDeletion = () => {};

    const handleEditBoardTitle = (value: string) => {
        if (value.trim() === '') return;
        setTitle(value.trim());
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
