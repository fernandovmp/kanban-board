import React, { useEffect, useState } from 'react';
import { AppBar } from '../../components';
import { Board } from '../../models';
import { kanbanServiceFactory } from '../../services/kanbanService';
import { BoardCard } from './BoardCard';
import { BoardList, CreateBoardCard, Main } from './styles';

export const UserBoards: React.FC = () => {
    const [boards, setBoards] = useState<Board[]>([]);

    useEffect(() => {
        const { getBoards } = kanbanServiceFactory();
        const _boards = getBoards();
        setBoards(_boards);
    }, []);

    const handleCreateBoard = () => {};

    return (
        <>
            <AppBar />
            <Main>
                <h2>Boards</h2>
                <BoardList>
                    <CreateBoardCard onClick={handleCreateBoard}>
                        Create a Board
                    </CreateBoardCard>
                    {boards.map((board) => (
                        <BoardCard key={board.id} board={board} />
                    ))}
                </BoardList>
            </Main>
        </>
    );
};
