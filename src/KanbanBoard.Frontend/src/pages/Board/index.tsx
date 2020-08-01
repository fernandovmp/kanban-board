import React, { useEffect, useState } from 'react';
import addIcon from '../../assets/add.svg';
import deleteIcon from '../../assets/delete_outline.svg';
import groupIcon from '../../assets/group.svg';
import { AppBar, DefaultButton, Input } from '../../components';
import { kanbanServiceFactory } from '../../services/kanbanService';
import { DeleteModal } from './DeleteModal';
import { MembersModal } from './MembersModal';
import {
    BoardTitle,
    Header,
    Main,
    NewListButton,
    TaskListsWrapper,
} from './styles';
import { TaskListView } from './TaskList';

export const BoardPage: React.FC = () => {
    const [board] = useState(() => kanbanServiceFactory().getBoard(1));
    const [boardTitle, setBoardTitle] = useState('');
    const [boardTitleInput, setBoardTitleInput] = useState('');
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [showMembersModal, setShowMembersModal] = useState(false);
    const [isEditingBoardTitle, setIsEditingBoardTitle] = useState(false);

    useEffect(() => {
        setBoardTitle(board?.summary ?? '');
    }, [board]);

    const handleMembersClick = () => {
        setShowMembersModal(true);
    };
    const handleDeleteBoard = () => {
        setShowDeleteModal(true);
    };
    const handleConfirmDeletion = () => {};
    const handleCreateList = () => {};

    const handleBeginEditBoardTitle = () => {
        setBoardTitleInput(boardTitle);
        setIsEditingBoardTitle(true);
    };

    const handleEditBoardTitle = () => {
        setIsEditingBoardTitle(false);
        if (boardTitleInput.trim() === '') return;
        setBoardTitle(boardTitleInput.trim());
    };

    return (
        <>
            <AppBar />
            <Main>
                <Header>
                    {isEditingBoardTitle ? (
                        <Input
                            value={boardTitleInput}
                            onChange={(e) => setBoardTitleInput(e.target.value)}
                            onBlur={handleEditBoardTitle}
                            autoFocus
                        />
                    ) : (
                        <BoardTitle onClick={handleBeginEditBoardTitle}>
                            {boardTitle}
                        </BoardTitle>
                    )}
                    <DefaultButton onClick={handleMembersClick}>
                        <img src={groupIcon} alt="Members" />
                        Members
                    </DefaultButton>
                    <DefaultButton onClick={handleDeleteBoard}>
                        <img src={deleteIcon} alt="Members" />
                        Delete board
                    </DefaultButton>
                </Header>
                <TaskListsWrapper>
                    {board?.lists?.map((list) => (
                        <TaskListView key={list.id} taskList={list} />
                    ))}
                    <NewListButton onClick={handleCreateList}>
                        <img src={addIcon} alt="New List" />
                        New list
                    </NewListButton>
                </TaskListsWrapper>
            </Main>
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
