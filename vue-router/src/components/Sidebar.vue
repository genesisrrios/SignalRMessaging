<template>
    <div id='sidepanel' v-bind:style="{'background-color':userPrimaryColor}">
    <div id='profile'>
        <div class='wrap'>
            <img id='profile-img' v-bind:src='pickRandomProfileImage()' v-bind:style="{'background-color':userPrimaryColor}" class='online' alt='profile image'>
            <p>{{username}}</p>
            <i class='fa fa-chevron-down expand-button' aria-hidden='true'></i>
            <div id='status-options'>
                <ul>
                <li id='status-online' class='active'><span class='status-circle'></span> <p>Online</p></li>
                </ul>
            </div>
        </div>
    </div>
    <div id='search'>
        <label for=''><i class='fa fa-search' aria-hidden='true'></i></label>
        <input v-bind:style="{'background-color':userPrimaryColor}" type='text' placeholder='Search contacts...' />
    </div>
    <div id='contacts'>
        <ul>
            <li class='contact active'>
                <div class='wrap'>
                <img src='http://emilcarlsson.se/assets/harveyspecter.png' alt='' />
                <div class='meta'>
                    <p class='name'>Harvey Specter</p>
                    <p class='preview'>Wrong. You take the gun, or you pull out a bigger one. Or, you call their bluff. Or, you do any one of a hundred and forty six other things.</p>
                </div>
                </div>
            </li>
            <hr>
        </ul>
    </div>
    <div id='bottom-bar'>
      <button data-toggle="modal" data-target="#modalComponent" v-bind:style="{'background-color':userPrimaryColor}"><i class='fa fa-user-plus fa-fw' aria-hidden='true'></i> <span>Add contact</span></button>
    </div >
  <searchmodal v-bind:primaryColor="userPrimaryColor"></searchmodal>
  </div>
</template>

<script>
import mixins from '../mixins.js'
import axios from 'axios'
import searchmodal from '@/components/SearchModal.vue'

export default {
  name: 'sidebar',
  components: {
    searchmodal
  },
  mixins: [mixins],
  data () {
    return {
      username: null,
      userPrimaryColor: null
    }
  },
  methods: {
    getUser: async function () {
      let self = this
      axios.get('https://localhost:44359/api/user/getnewuser')
        .then(function (response) {
          let data = response.data.Values
          self.assignValuesToUserInformation(data)
        }).catch(error => {
          console.log(error.response)
        })
    },
    assignValuesToUserInformation: function (parameters) {
      this.username = parameters.user_name
    },
    pickRandomColorForUser: function () {
      let colorListLength = hexColorList.length
      let randomNumber = generateRandomNumber(colorListLength)
      this.userPrimaryColor = hexColorList[randomNumber]
    },
    pickRandomProfileImage: function () {
      let imageListLength = imageList.length
      let randomNumber = generateRandomNumber(imageListLength)
      return require('../assets/' + imageList[randomNumber])
    }
  },
  created: function () {
    this.getUser()
    this.pickRandomColorForUser()
  }
}
function generateRandomNumber (maxNumber = 10) {
  let min = Math.ceil(0)
  let max = Math.floor(maxNumber)
  return Math.floor(Math.random() * (max - min + 1)) + min
}
/* eslint-disable */ 
let hexColorList = ['#f0f8ff', '#00ffff', '#7fffd4', '#f0ffff', '#f5f5dc', '#ffe4c4',
                '#000000', '#ffebcd', '#0000ff', '#8a2be2', '#a52a2a', '#deb887', '#5f9ea0',
                '#7fff00', '#d2691e', '#ff7f50', '#6495ed', '#fff8dc', '#dc143c', '#00ffff',
                '#00008b', '#008b8b', '#b8860b', '#a9a9a9', '#006400', '#a9a9a9', '#bdb76b',
                '#8b008b', '#556b2f', '#ff8c00', '#9932cc', '#8b0000', '#8fbc8f', '#483d8b',
                '#2f4f4f', '#2f4f4f', '#00ced1', '#9400d3', '#ff1493', '#00bfff', '#696969',
                '#696969', '#1e90ff', '#b22222', '#228b22', '#ff00ff', '#dcdcdc', '#daa520',
                '#f0fff0', '#ff69b4', '#cd5c5c', '#4b0082', '#f0e68c', '#ffd700', '#808080',
                '#e6e6fa', '#7cfc00', '#fffacd', '#add8e6', '#f08080', '#9acd32', '#fafad2',
                '#d3d3d3', '#90ee90', '#d3d3d3', '#ffb6c1', '#ffa07a', '#20b2aa', '#87cefa',
                '#778899', '#778899', '#b0c4de', '#ffffe0', '#00ff00', '#32cd32', '#faf0e6',
                '#ff00ff', '#800000', '#66cdaa', '#0000cd', '#ba55d3', '#9370db', '#3cb371',
                '#7b68ee', '#00fa9a', '#48d1cc', '#c71585', '#f5fffa', '#ffe4e1', '#ffe4b5',
                '#ffdead', '#000080', '#808000', '#6b8e23', '#ffa500', '#ff4500', '#ffff00',
                '#da70d6', '#eee8aa', '#98fb98', '#afeeee', '#db7093', '#ffdab9', '#008000',
                '#cd853f', '#ffc0cb', '#dda0dd', '#b0e0e6', '#800080', '#663399', '#ff0000',
                '#bc8f8f', '#4169e1', '#8b4513', '#fa8072', '#f4a460', '#2e8b57', '#808080',
                '#a0522d', '#c0c0c0', '#87ceeb', '#6a5acd', '#708090', '#708090', '#adff2f',
                '#00ff7f', '#4682b4', '#d2b48c', '#008080', '#d8bfd8', '#ff6347', '#40e0d0',
                '#ee82ee', '#f5deb3'] 
                
let imageList = ['analytics-graph-bar.svg', 'baby-trolley.svg', 'baggage.svg', 
                'beach-parasol-water-1.svg', 'biking-person.svg', 'bin-2.svg', 
                'binocular.svg', 'bomb-grenade.svg', 'building-modern-1.svg', 
                'camera-flash.svg', 'coffee-cold.svg', 'coffee-machine.svg', 
                'cog.svg', 'content-ink-pen-write.svg', 'content-pen-3.svg', 
                'content-typing-machine.svg', 'couple-man-woman.svg', 'coupon-cut.svg', 
                'crypto-currency-bitcoin-imac.svg', 'dating-rose.svg', 'dating-smartphone-man.svg', 
                'design-tool-pen-station.svg', 'design-tool-quill-2.svg', 
                'desktop-monitor-download.svg', 'desktop-monitor-upload.svg', 
                'diet-waist-1.svg', 'e-learning-monitor.svg', 'email-action-receive.svg', 
                'filter-picture.svg', 'flag.svg', 'footwear-heels-ankle.svg', 'gauge-dashboard-1.svg', 
                'golf-hole.svg', 'graph-stats-circle.svg', 'hammer-1.svg', 'headphones-human.svg', 
                'human-resources-search-employees.svg', 'insurance-card.svg', 'lab-flask-experiment.svg', 
                'landmark-japan-shrine.svg', 'laptop-smiley-1.svg', 'lighter.svg', 'like-1.svg', 
                'login-2.svg', 'love-boat.svg', 'maps-pin.svg', 'medical-personnel-doctor.svg', 
                'megaphone-1.svg', 'messages-people-person-bubble-oval.svg', 'mobile-phone-2.svg', 
                'modern-tv-3d-glasses.svg', 'money-wallet-open.svg', 'monkey.svg', 'mood-happy.svg', 
                'mouse.svg', 'nautic-sports-sailing-person.svg', 'office-work-wireless.svg', 
                'optimization-timer-1.svg', 'outdoors-tree-valley.svg', 'pencil-2.svg', 
                'people-man-graduate.svg', 'people-woman-glasses-1.svg', 'phone-actions-ring.svg', 
                'pin-monitor.svg', 'pin.svg', 'plane-trip-international.svg', 
                'presentation-board-graph.svg', 'print-text.svg', 'programming-flag.svg', 'rat.svg', 
                'rooster.svg', 'science-dna.svg', 'screen-1.svg', 'send-email-2.svg', 
                'shipment-upload.svg', 'shop-cashier-man.svg', 'shop-cashier-woman.svg', 
                'shopping-bag-heart.svg', 'shopping-bag-tag-1.svg', 'shopping-cart-download.svg', 
                'show-hat-magician-1.svg', 'skull-1.svg', 'skunk.svg', 'stamps-famous.svg', 
                'tablet-touch.svg', 'taking-pictures-circle.svg', 'target-center-2.svg', 
                'video-game-gamasutra.svg', 'video-game-mario-3.svg', 'video-game-mario-enemy.svg', 
                'video-player-1.svg', 'video-player-cloud.svg', 'video-player-monitor.svg', 
                'view-off.svg', 'view.svg', 'warehouse-cart-packages.svg', 'warehouse-storage-3.svg', 
                'wench-double.svg', 'whale-body.svg', 'wireless-payment-credit-card-dollar.svg']                
</script>

<style>
</style>
