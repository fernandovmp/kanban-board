import styled from 'styled-components';
import { Card, IconButton } from '..';

export const OverlayWrapper = styled(Card)`
    position: fixed;
    padding: 12px;
    display: flex;
    flex-direction: column;
    justify-content: center;
`;

export const Header = styled.header`
    font-weight: bold;
    display: flex;
    flex-direction: column;
    justify-content: flex-end;
`;
export const Main = styled.main`
    margin-top: 10px;
`;
export const Separator = styled.div`
    background: var(--text);
    height: 1px;
    width: 100%;
    margin: 0;
`;

export const CloseButton = styled(IconButton)`
    align-self: flex-end;
    margin-bottom: 10px;
`;
