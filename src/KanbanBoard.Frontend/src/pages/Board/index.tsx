import React, { useState } from 'react';
import deleteIcon from '../../assets/delete_outline.svg';
import groupIcon from '../../assets/group.svg';
import { AppBar } from '../../components';
import { kanbanServiceFactory } from '../../services/kanbanService';
import { BoardTitle, Button, Header, Main } from './styles';

export const BoardPage: React.FC = () => {
    const [board] = useState(() => kanbanServiceFactory().getBoards()[0]);

    const handleMembersClick = () => {};
    const handleDeleteBoard = () => {};

    return (
        <>
            <AppBar />
            <Main>
                <Header>
                    <BoardTitle>{board.summary}</BoardTitle>
                    <Button onClick={handleMembersClick}>
                        <img src={groupIcon} alt="Members" />
                        Members
                    </Button>
                    <Button onClick={handleDeleteBoard}>
                        <img src={deleteIcon} alt="Members" />
                        Delete board
                    </Button>
                </Header>
            </Main>
        </>
    );
};
