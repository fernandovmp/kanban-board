import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Board } from '../../models';
import {
    apiGet,
    apiPost,
    isErrorResponse,
} from '../../services/kanbanApiService';
import { BoardCard } from './BoardCard';
import { BoardList, CreateBoardCard, Main } from './styles';

export const UserBoards: React.FC = () => {
    const [boards, setBoards] = useState<Board[]>([]);
    const history = useHistory();

    useEffect(() => {
        const fetchUserBoards = async () => {
            const token = sessionStorage.getItem('jwtToken');
            if (!token) {
                history.push('/login');
                return;
            }
            const response = await apiGet<Board[]>({
                uri: 'v1/boards',
                bearerToken: token,
            });
            if (isErrorResponse(response.data)) {
                if (response.data.status === 401) {
                    history.push('/login');
                }
                return;
            }
            const _boards = response.data!;
            setBoards(_boards);
        };
        fetchUserBoards();
    }, [history]);

    const handleCreateBoard = async () => {
        const token = sessionStorage.getItem('jwtToken');
        if (!token) {
            history.push('/login');
            return;
        }

        const response = await apiPost<Board>({
            uri: 'v1/boards',
            body: {
                title: 'untitled',
            },
            bearerToken: token,
        });

        if (isErrorResponse(response.data)) {
            if (response.data.status === 401) {
                history.push('/login');
                return;
            }
            alert('error on create the board');
            return;
        }

        const board = response.data!;

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
