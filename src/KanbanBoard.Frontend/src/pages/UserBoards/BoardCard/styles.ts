import styled from 'styled-components';
import { Card } from '../../../components';

export const SBoardCard = styled(Card)`
    width: 200px;
    height: 80px;
    padding: 8px;

    :hover {
        cursor: pointer;
    }
`;

export const BoardTitle = styled.strong`
    font-size: medium;
`;
