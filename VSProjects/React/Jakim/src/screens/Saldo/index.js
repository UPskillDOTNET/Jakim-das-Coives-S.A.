import React, { useEffect, useState, Component} from 'react';
import {Text, View, AsyncStorage, FlatList} from 'react-native';
import {useNavigation, useRoute, useFocusEffect} from '@react-navigation/native';

import {Container} from './styles';
import Api from '../../Api'

export default () => {
    return (
        <Container>
            <Text>Reservas</Text>
        </Container>
    );
}