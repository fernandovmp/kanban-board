import styled from 'styled-components';
import { Card, DefaultButton } from '../../../components';

export const ListWrapper = styled(Card)`
    background: #f0f0f0;
    width: 240px;
    padding: 10px 6px;
    display: flex;
    flex-direction: column;
    gap: 10px;
`;

export const ListTitle = styled.h3`
    margin: 0;
`;

export const Button = styled(DefaultButton)`
    background: transparent;
    font-size: small;
`;
