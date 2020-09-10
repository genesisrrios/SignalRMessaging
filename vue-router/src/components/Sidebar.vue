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
        <ul v-for="contact in contactList" v-bind:key="contact.user_id">
            <li class='contact'>
                <div class='wrap'>
                <div class='meta'>
                    <p class='name'>{{contact.user_name}}</p>
                    <p class='preview'>{{contact.last_message_excerpt}}</p>
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
import { EventBus } from '../event-bus.js'

export default {
  name: 'sidebar',
  components: {
    searchmodal
  },
  mixins: [mixins],
  data () {
    return {
      username: null,
      userPrimaryColor: null,
      contactList: []
    }
  },
  methods: {
    getUser: async function () {
      let self = this
      if (localStorage.username && localStorage.userId) {
        axios.get(`${this.apiUrl}api/user/getuser?user_id=${localStorage.userId}`)
          .then(function (response) {
            if (!response.data.values) {
              self.createNewUser()
            } else {
              self.assignValuesToUserInformation(response.data.values)
              self.getContactList()
            }
          }).catch(error => {
            console.log(error)
          })
      } else {
        self.createNewUser()
      }
    },
    assignValuesToUserInformation: function (parameters) {
      localStorage.userId = parameters.id
      localStorage.username = parameters.user_name
      this.username = parameters.user_name
      this.userPrimaryColor = parameters.primary_color_hex
    },
    pickRandomProfileImage: function () {
      let imageListLength = imageList.length
      let randomNumber = generateRandomNumber(imageListLength)
      return require('../assets/' + imageList[randomNumber])
    },
    createNewUser: function () {
      let self = this
      // eslint-disable-next-line
      axios.get(`${this.apiUrl}api/user/getnewuser`)
        .then(function (response) {
          let data = response.data.values
          self.assignValuesToUserInformation(data)
        }).catch(error => {
          console.log(error)
        })
    },
    getContactList: function () {
      let self = this
        axios.get(`${this.apiUrl}api/contact/getContacts?user_id=${localStorage.userId}`)
          .then(function (response) {
            if (response.data.values) {
              self.contactList = response.data.values
            }
            else{

            }
          }).catch(error => {
            console.log(error)
          })
    }
  },
  created: function () {    
    let self = this
    EventBus.$on('contact-added', function(){
      self.getContactList();
    })
  },
  mounted: function () {
    this.getUser()
  }
}


function generateRandomNumber (maxNumber = 10) {
  let min = Math.ceil(0)
  let max = Math.floor(maxNumber)
  return Math.floor(Math.random() * (max - min + 1)) + min
}

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
