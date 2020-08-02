import React, { useEffect, useState } from 'react';
import addIcon from '../../assets/add.svg';
import deleteIcon from '../../assets/delete_outline.svg';
import groupIcon from '../../assets/group.svg';
import { AppBar, DefaultButton, Input } from '../../components';
import { EditableContent } from '../../components/EditableContent';
import { TaskList } from '../../models';
import { kanbanServiceFactory } from '../../services/kanbanService';
import { DeleteModal } from './DeleteModal';
import { MembersModal } from './MembersModal';
import {
    BoardTitle,
    ButtonsWrapper,
    CancelButton,
    Header,
    Main,
    NewListButton,
    SaveButton,
    TaskListsWrapper,
} from './styles';
import { TaskListView } from './TaskList';
import { ListWrapper } from './TaskList/styles';

export const BoardPage: React.FC = () => {
    const [board] = useState(() => kanbanServiceFactory().getBoard(1));
    const [boardTitle, setBoardTitle] = useState('');
    const [boardLists, setBoardLists] = useState<TaskList[]>([]);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [showMembersModal, setShowMembersModal] = useState(false);
    const [newListTitle, setNewListTitle] = useState('');
    const [isCreatingList, setIsCreatingList] = useState(false);

    useEffect(() => {
        setBoardTitle(board?.summary ?? '');
        setBoardLists(board?.lists ?? []);
    }, [board]);

    const handleMembersClick = () => {
        setShowMembersModal(true);
    };
    const handleDeleteBoard = () => {
        setShowDeleteModal(true);
    };
    const handleConfirmDeletion = () => {};

    const handleCreateList = () => {
        setIsCreatingList(false);
        if (newListTitle.trim() === '') return;
        const taskList: TaskList = {
            id: 70,
            title: newListTitle.trim(),
            tasks: [],
        };
        setNewListTitle('');
        setBoardLists([...boardLists, taskList]);
    };

    const handleEditBoardTitle = (value: string) => {
        if (value.trim() === '') return;
        setBoardTitle(value.trim());
    };

    return (
        <>
            <AppBar />
            <Main>
                <Header>
                    <EditableContent
                        initialInputValue={boardTitle}
                        onEndEdit={handleEditBoardTitle}
                    >
                        <BoardTitle>{boardTitle}</BoardTitle>
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
                <TaskListsWrapper>
                    {boardLists.map((list) => (
                        <TaskListView key={list.id} taskList={list} />
                    ))}
                    {isCreatingList ? (
                        <ListWrapper>
                            <Input
                                autoFocus
                                value={newListTitle}
                                onChange={(e) =>
                                    setNewListTitle(e.target.value)
                                }
                            />
                            <ButtonsWrapper>
                                <CancelButton
                                    onClick={() => setIsCreatingList(false)}
                                >
                                    CANCEL
                                </CancelButton>
                                <SaveButton onClick={handleCreateList}>
                                    SAVE
                                </SaveButton>
                            </ButtonsWrapper>
                        </ListWrapper>
                    ) : (
                        <NewListButton onClick={() => setIsCreatingList(true)}>
                            <img src={addIcon} alt="New List" />
                            New list
                        </NewListButton>
                    )}
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
