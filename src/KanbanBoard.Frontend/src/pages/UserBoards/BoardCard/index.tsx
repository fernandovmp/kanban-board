import React from 'react';
import { useHistory } from 'react-router-dom';
import { Board } from '../../../models';
import { BoardTitle, SBoardCard } from './styles';

interface IBoardCardProps {
    board: Board;
}

export const BoardCard: React.FC<IBoardCardProps> = ({ board }) => {
    const history = useHistory();

    const handleClick = () => {
        history.push(`/board/${board.id}`);
    };

    return (
        <SBoardCard onClick={handleClick}>
            <BoardTitle>{board.title}</BoardTitle>
        </SBoardCard>
    );
};
