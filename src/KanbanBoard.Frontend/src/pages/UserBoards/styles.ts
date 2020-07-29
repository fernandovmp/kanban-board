import styled from 'styled-components';
import { SBoardCard } from './BoardCard/styles';

export const Main = styled.main`
    padding: 20px 100px;
`;

export const BoardList = styled.div`
    display: flex;
    flex-wrap: wrap;
    gap: 15px;
`;

export const CreateBoardCard = styled(SBoardCard)`
    background: #ffff6b;
    display: flex;
    align-items: center;
    justify-content: center;
`;
