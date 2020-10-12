import styled from 'styled-components';
import { Card } from '../../../../components';

export const TaskWrapper = styled(Card)`
    display: flex;
    flex-direction: column;
    padding: 5px 8px;
    gap: 6px;
    font-size: smaller;
    &:hover {
        cursor: pointer;
    }
    word-wrap: break-word;
`;

interface ITagProps {
    color: string;
}

export const Tag = styled.div<ITagProps>`
    border-radius: 8px;
    background-color: ${(props) => props.color};
    height: 4px;
`;
