import React, { useEffect, useState } from 'react';
import addIcon from '../../assets/add.svg';
import { AppBar, Input } from '../../components';
import { TaskList } from '../../models';
import { kanbanServiceFactory } from '../../services/kanbanService';
import { BoardHeader } from './BoardHeader';
import {
    ButtonsWrapper,
    CancelButton,
    Main,
    NewListButton,
    SaveButton,
    TaskListsWrapper,
} from './styles';
import { TaskListView } from './TaskList';
import { ListWrapper } from './TaskList/styles';

export const BoardPage: React.FC = () => {
    const [board] = useState(() => kanbanServiceFactory().getBoard(1));
    const [boardLists, setBoardLists] = useState<TaskList[]>([]);
    const [newListTitle, setNewListTitle] = useState('');
    const [isCreatingList, setIsCreatingList] = useState(false);

    useEffect(() => {
        setBoardLists(board?.lists ?? []);
    }, [board]);

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

    return (
        <>
            <AppBar />
            <Main>
                <BoardHeader boardTitle={board?.summary ?? ''} />
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
        </>
    );
};
