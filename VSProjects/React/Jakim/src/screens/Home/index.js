import React, {useState, useEffect} from 'react';
import { useNavigation } from '@react-navigation/native';

import ParqueItem from '../../components/ParqueItem';
import Api from '../../Api';
import {Container,
        Scroller,

        HeaderArea,
        HeaderTitle,
        SearchButton,

        LocationArea,
        LocationInput,
        LocationFinder,

        LoadingIcon,
        ListArea
} from './styles';

import SearchIcon from '../../assets/search.svg';
import LocationIcon from '../../assets/my_location.svg';

export default () => {

    const navigation = useNavigation();

    const [locationText, setLocationText]= useState('');
    const [loading, setLoading] = useState(false);
    //feeling cute, might delete later
    const [list, setList] = useState([]);
    const getReservas = async () => {
       setLoading(true);
       setList([]);
       
       let res = await Api.getReservas();
       if(res.error == ''){
        if(res.loc){
            setLocationText(res.loc);
        }
        setList(res.data);

       }else{
           alert("Erro: "+res.error);
       }

       setLoading(false);
    }

    useEffect(()=>{
        getReservas();
    },[]);

    return (
        <Container>
            <Scroller>

                <HeaderArea>
                    <HeaderTitle numberOfLines={2}>Estacione no seu parque favorito</HeaderTitle>
                    <SearchButton onPress={()=> navigation.navigate('Search')}>
                        <SearchIcon width="26" height="26" fill="#FFFFFF"/>
                    </SearchButton>
                </HeaderArea>

                <LocationArea>
                    <LocationInput 
                    placeholder="A sua Localização"
                    placeholderTextColor="#FFFFFF"
                    value= {locationText}
                    onChangeText={t=>setLocationText(t)}
                    />

                    <LocationFinder>
                        <LocationIcon width="26" height="26" fill="#FFFFFF"/>
                    </LocationFinder>
                </LocationArea>

                {loading &&
                <LoadingIcon size="large" color="#FFFFFF"/>
                }

                <ListArea>
                    {list.map((item, k)=>(
                        <ParqueItem key={k} data={item}/>
                    ))}
                </ListArea>

            </Scroller>
        </Container>
    );
}
