import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Board } from '../../models';
import { isErrorResponse, postBoard } from '../../services/kanbanApiService';
import { kanbanServiceFactory } from '../../services/kanbanService';
import { BoardCard } from './BoardCard';
import { BoardList, CreateBoardCard, Main } from './styles';

export const UserBoards: React.FC = () => {
    const [boards, setBoards] = useState<Board[]>([]);
    const history = useHistory();

    useEffect(() => {
        const { getBoards } = kanbanServiceFactory();
        const _boards = getBoards();
        setBoards(_boards);
    }, []);

    const handleCreateBoard = async () => {
        const token = sessionStorage.getItem('jwtToken');
        if (!token) {
            history.push('/login');
            return;
        }

        const response = await postBoard(
            {
                title: 'untitled',
            },
            token
        );

        if (isErrorResponse(response.data)) {
            if (response.data.status === 401) {
                history.push('/login');
                return;
            }
            alert('error on create the board');
            return;
        }

        const board = response.data;

        history.push(`/board/${board.id}`);
    };

    return (
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
    );
};
