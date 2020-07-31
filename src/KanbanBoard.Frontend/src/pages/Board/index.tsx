import React, { useState } from 'react';
import deleteIcon from '../../assets/delete_outline.svg';
import groupIcon from '../../assets/group.svg';
import { AppBar, DefaultButton } from '../../components';
import { kanbanServiceFactory } from '../../services/kanbanService';
import { BoardTitle, Header, Main, TaskListsWrapper } from './styles';
import { TaskListView } from './TaskList';

export const BoardPage: React.FC = () => {
    const [board] = useState(() => kanbanServiceFactory().getBoard(1));

    const handleMembersClick = () => {};
    const handleDeleteBoard = () => {};

    return (
        <>
            <AppBar />
            <Main>
                <Header>
                    <BoardTitle>{board?.summary}</BoardTitle>
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
                </TaskListsWrapper>
            </Main>
        </>
    );
};
